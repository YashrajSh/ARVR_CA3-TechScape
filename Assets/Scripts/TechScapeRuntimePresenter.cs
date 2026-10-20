using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Preserves the original scene, Vuforia, and map setup while replacing the
/// old culture-facing content at runtime with the TechScape dataset.
/// </summary>
public static class TechScapeRuntimePresenter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Register()
    {
        SceneManager.sceneLoaded -= PresentScene;
        SceneManager.sceneLoaded += PresentScene;
        PresentScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private static void PresentScene(Scene scene, LoadSceneMode mode)
    {
        var state = TechScapeStateData.GetState(scene.name);
        if (state != null)
        {
            TechScapeStateData.SelectState(state.stateId);
            ApplyStateContent(state);
        }

        ReplaceBranding();
    }

    private static void ApplyStateContent(TechState state)
    {
        SetTextOnObject("Heading", state.stateName + " | Technology Ecosystem");
        SetTextOnObject("Content", BuildDetailText(state));
        ConfigureDetailScroll();
        SetTextOnObject("Know More", "Tech Details");

        var visual = Resources.Load<Texture2D>("TechScapeScanTarget");
        if (visual == null)
            return;

        var sprite = Sprite.Create(visual, new Rect(0, 0, visual.width, visual.height), new Vector2(0.5f, 0.5f));
        foreach (var image in Resources.FindObjectsOfTypeAll<Image>())
        {
            if (image.gameObject.scene.name == SceneManager.GetActiveScene().name && image.gameObject.name == "Image")
            {
                image.sprite = sprite;
                image.preserveAspect = true;
            }
        }
    }

    private static string BuildDetailText(TechState state)
    {
        var sections = new System.Collections.Generic.List<string>();
        AddSection(sections, "TECH OVERVIEW", state.techOverview);
        AddSection(sections, "CITY SPOTLIGHTS", Valid(state.citySpotlights, "\n"));
        AddSection(sections, "KEY SECTORS", Valid(state.majorTechSectors, " • "));
        AddSection(sections, "COMPANIES & STARTUPS", Valid(state.notableCompanies, " • ") + "\n" + Valid(state.notableStartups, " • "));
        AddSection(sections, "INNOVATION & RESEARCH", Valid(state.researchInstitutions, " • ") + "\n" + Valid(state.innovationHubs, " • "));
        AddSection(sections, "INITIATIVES", Valid(state.majorInitiatives, " • "));
        AddSection(sections, "TECH FACT", state.interestingFact);
        return string.Join("\n\n", sections);
    }

    private static void AddSection(System.Collections.Generic.List<string> sections, string heading, string value)
    {
        value = value?.Trim();
        if (!string.IsNullOrEmpty(value)) sections.Add($"<b>{heading}</b>\n{value}");
    }

    private static string Valid(string[] values, string separator)
    {
        if (values == null) return string.Empty;
        var kept = new System.Collections.Generic.List<string>();
        foreach (var value in values)
            if (!string.IsNullOrWhiteSpace(value) && !IsPlaceholder(value)) kept.Add(value.Trim());
        return string.Join(separator, kept);
    }

    private static bool IsPlaceholder(string value)
    {
        var text = value.Trim().ToLowerInvariant();
        return text == "n/a" || text == "tbd" || text.Contains("placeholder") || text.Contains("coming soon") || text.Contains("information unavailable") || text.Contains("no data") || text.Contains("to be verified") || text.Contains("in progress") || text.Contains("lorem ipsum") || text.Contains("sample text") || text.Contains("add description") || text.Contains("default content") || text == "unknown";
    }

    private static void ConfigureDetailScroll()
    {
        var scroll = Resources.FindObjectsOfTypeAll<ScrollRect>();
        foreach (var rect in scroll)
        {
            if (rect.gameObject.scene.name != SceneManager.GetActiveScene().name || rect.content == null) continue;
            var text = FindText("Content");
            if (text == null) continue;
            text.rectTransform.SetParent(rect.content, false);
            text.rectTransform.anchorMin = new Vector2(0, 1); text.rectTransform.anchorMax = new Vector2(1, 1); text.rectTransform.pivot = new Vector2(.5f, 1);
            var height = text.GetPreferredValues(text.text, rect.viewport.rect.width, 0).y + 28f;
            text.rectTransform.sizeDelta = new Vector2(0, height);
            rect.content.sizeDelta = new Vector2(0, height);
            rect.horizontal = false; rect.vertical = true; rect.enabled = true; rect.scrollSensitivity = 20f;
            Canvas.ForceUpdateCanvases(); rect.verticalNormalizedPosition = 1f;
            break;
        }
    }

    private static TMP_Text FindText(string name)
    {
        foreach (var text in Resources.FindObjectsOfTypeAll<TMP_Text>())
            if (text.gameObject.scene.name == SceneManager.GetActiveScene().name &&
                (text.gameObject.name == name || (text.transform.parent != null && text.transform.parent.name == name))) return text;
        return null;
    }

    private static void ReplaceBranding()
    {
        foreach (var button in Resources.FindObjectsOfTypeAll<Button>())
            if (button.gameObject.scene.name == SceneManager.GetActiveScene().name &&
                (button.gameObject.name.IndexOf("Sanskriti", StringComparison.OrdinalIgnoreCase) >= 0 ||
                 button.gameObject.name.IndexOf("TechMitra", StringComparison.OrdinalIgnoreCase) >= 0 ||
                 button.gameObject.name.IndexOf("Ask", StringComparison.OrdinalIgnoreCase) >= 0))
                button.gameObject.SetActive(false);

        foreach (var text in Resources.FindObjectsOfTypeAll<TMP_Text>())
        {
            if (!text.gameObject.scene.IsValid() || text.gameObject.scene.name != SceneManager.GetActiveScene().name)
                continue;

            if (!string.IsNullOrEmpty(text.text))
            {
                text.text = text.text.Replace("Sanskriti", "TechMitra")
                    .Replace("Kala Kunj", "TechScape India")
                    .Replace("KalaKunj", "TechScape India")
                    .Replace("Kala Kriti", "TechScape India")
                    .Replace("KalaKriti", "TechScape India");
            }
        }
    }

    private static void SetTextOnObject(string objectName, string value)
    {
        foreach (var text in Resources.FindObjectsOfTypeAll<TMP_Text>())
        {
            if (text.gameObject.scene.name == SceneManager.GetActiveScene().name &&
                (text.gameObject.name == objectName || (text.transform.parent != null && text.transform.parent.name == objectName)))
                text.text = value;
                if (objectName == "Content")
                {
                    text.enableAutoSizing = true;
                    text.fontSizeMin = 17;
                    text.fontSizeMax = 25;
                }
        }
    }
}
