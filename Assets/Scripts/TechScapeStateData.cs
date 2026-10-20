using System;
using UnityEngine;

/// <summary>
/// Loads the state-wise TechScape dataset from Resources and keeps the state
/// selected on the AR map available to detail screens and TechMitra.
/// </summary>
public static class TechScapeStateData
{
    private const string ResourceName = "TechScapeStateData";
    private const string SelectedStateKey = "TechScape.SelectedState";
    private static readonly string[] SupportedMapScenes =
    {
        "AndhraPradesh", "ArunachalPradesh", "Assam", "Bihar", "Chhattisgarh", "Goa", "Gujarat", "Haryana",
        "HP", "IslandAndaman", "J&k", "Jharkhand", "Karnataka", "Kerala", "Ladakh", "Lakshwadeep", "Maharashtra",
        "Manipur", "Meghalaya", "Mizoram", "MP", "Nagaland", "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu",
        "Telangana", "Tripura", "Uttar Pradesh", "Uttarakhand", "West Bengal"
    };
    private static TechStateCollection cachedCollection;

    public static void SelectState(string stateId)
    {
        PlayerPrefs.SetString(SelectedStateKey, stateId ?? string.Empty);
        PlayerPrefs.Save();
    }

    public static string SelectedStateId => PlayerPrefs.GetString(SelectedStateKey, string.Empty);

    public static bool IsSupportedState(string stateId)
    {
        return GetState(stateId) != null;
    }

    public static TechState GetSelectedState()
    {
        return GetState(SelectedStateId);
    }

    public static TechState GetState(string stateId)
    {
        if (string.Equals(stateId, "Gujrat", StringComparison.OrdinalIgnoreCase)) stateId = "Gujarat";
        EnsureLoaded();
        if (cachedCollection == null || cachedCollection.states == null || string.IsNullOrEmpty(stateId))
            return null;

        foreach (var state in cachedCollection.states)
        {
            if (string.Equals(state.stateId, stateId, StringComparison.OrdinalIgnoreCase))
                return state;
        }

        return null;
    }

    public static string BuildChatContext(TechState state)
    {
        if (state == null)
            return "No state is currently selected. Ask a question about India's technology and innovation ecosystem.";

        return $"Selected state: {state.stateName}. Overview: {state.techOverview} " +
               $"Hubs: {string.Join(", ", state.primaryTechHubs)}. " +
               $"City spotlights: {string.Join(" ", state.citySpotlights)}. " +
               $"Sectors: {string.Join(", ", state.majorTechSectors)}. " +
               $"Companies and startups: {string.Join(", ", state.notableCompanies)}; {string.Join(", ", state.notableStartups)}. " +
               $"Innovation: {string.Join(", ", state.innovationHubs)}. Fact: {state.interestingFact}";
    }

    private static void EnsureLoaded()
    {
        if (cachedCollection != null)
            return;

        var dataAsset = Resources.Load<TextAsset>(ResourceName);
        if (dataAsset == null)
        {
            Debug.LogWarning("TechScape state data could not be found in Resources.");
            return;
        }

        cachedCollection = JsonUtility.FromJson<TechStateCollection>(dataAsset.text);
    }

}

[Serializable]
public class TechStateCollection
{
    public TechState[] states;
}

[Serializable]
public class TechState
{
    public string stateId;
    public string stateName;
    public string shortDescription;
    public string techOverview;
    public string[] primaryTechHubs;
    public string[] citySpotlights;
    public string[] majorTechSectors;
    public string[] notableCompanies;
    public string[] notableStartups;
    public string[] notableInnovators;
    public string[] researchInstitutions;
    public string[] innovationHubs;
    public string[] majorInitiatives;
    public string interestingFact;
    public string[] imageReferences;
    public string chatbotContext;
}
