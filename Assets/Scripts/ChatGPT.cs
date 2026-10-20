using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;



namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text responseText;
        [Tooltip("Optional. Configure this in the Unity Inspector for API responses; never commit a real key.")]
        [SerializeField] private string apiKey = "";

        private OpenAIApi openai;
        private List<ChatMessage> messages = new List<ChatMessage>();
        private const string BasePrompt = "You are TechMitra, the concise and student-friendly technology guide for TechScape India, an AR application about India's technology and innovation ecosystem. Focus on Indian technology, explain terms plainly, use the selected state as context when the user says here or this state, do not invent companies, founders or initiatives, and state uncertainty when needed.";



        private void Start()
        {
            button.onClick.AddListener(SendReply);
            var configuredKey = string.IsNullOrWhiteSpace(apiKey)
                ? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                : apiKey;

            if (!string.IsNullOrWhiteSpace(configuredKey) && configuredKey != "YOUR_API_KEY_HERE")
                openai = new OpenAIApi(configuredKey);

            AppendMessage("Hi, I’m TechMitra. Ask me about a state’s technology ecosystem or a technology term.");
        }

        private void AppendMessage(string content)
        {
            responseText.text += content + "\n" + "\n";
            Canvas.ForceUpdateCanvases();

        }

        private async void SendReply()
        {
            var userMessage = inputField.text.Trim();
            if (string.IsNullOrEmpty(userMessage))
                return;

            var selectedState = TechScapeStateData.GetSelectedState();
            var context = TechScapeStateData.BuildChatContext(selectedState);

            if (openai == null)
            {
                AppendMessage(CreateOfflineReply(userMessage, selectedState));
                inputField.text = "";
                return;
            }

            if (messages.Count == 0)
                messages.Add(new ChatMessage { Role = "system", Content = BasePrompt + "\n" + context });

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = userMessage
            };

            messages.Add(newMessage);

            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            try
            {
                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-6-astra",
                    Messages = messages
                });

                if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    messages.Add(message);
                    AppendMessage(message.Content);
                }
                else
                {
                    AppendMessage("I could not generate a response just now. Please try again or use the offline guide.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("TechMitra API request failed: " + exception.Message);
                AppendMessage(CreateOfflineReply(userMessage, selectedState));
            }

            button.enabled = true;
            inputField.enabled = true;
        }

        private static string CreateOfflineReply(string question, TechState state)
        {
            var normalizedQuestion = question.ToLowerInvariant();
            if (normalizedQuestion.Contains("saas"))
                return "SaaS means Software as a Service: software delivered over the internet, usually by subscription, instead of installed and maintained locally.";
            if (normalizedQuestion.Contains("fintech"))
                return "FinTech means financial technology: digital products or platforms that improve payments, lending, investing, insurance or other financial services.";
            if (normalizedQuestion.Contains("cybersecurity"))
                return "Cybersecurity is the practice of protecting devices, networks, software and data from unauthorized access or attacks.";
            if (normalizedQuestion.Contains("semiconductor"))
                return "Semiconductors are materials used to make electronic chips. Chip design, fabrication, packaging and testing together form the semiconductor ecosystem.";
            if (normalizedQuestion.Contains("incubator"))
                return "A startup incubator helps early teams with mentoring, workspace, networks and sometimes funding or technical support.";
            if (normalizedQuestion.Contains(" ai") || normalizedQuestion.StartsWith("ai") || normalizedQuestion.Contains("artificial intelligence"))
                return "Artificial intelligence is a field of computing in which systems learn patterns from data to support tasks such as prediction, language processing, vision or decision-making.";
            if (state != null)
                return $"{state.stateName}: {state.shortDescription}\n\nTech hubs: {string.Join(", ", state.primaryTechHubs)}.\nKey sectors: {string.Join(", ", state.majorTechSectors)}.\nCompanies: {string.Join(", ", state.notableCompanies)}.\nStartups: {string.Join(", ", state.notableStartups)}.\nInnovation: {string.Join(", ", state.innovationHubs)}.\n\n{state.interestingFact}";

            return "Select a state on the AR map first, or ask about a term such as AI, SaaS, FinTech, cybersecurity, semiconductors or startup incubators.";
        }
    }
}
