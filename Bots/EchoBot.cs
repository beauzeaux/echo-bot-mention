// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var reply = await CreateReplyActivityAsync(turnContext.Activity.From);
            Console.Out.WriteLine(JsonConvert.SerializeObject(reply));
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        private async Task<IMessageActivity> CreateReplyActivityAsync(ChannelAccount user) {
            var templatePath = Path.Combine(".", "Resources", "EchoCard.json");
            var templateJson = await File.ReadAllTextAsync(templatePath);
            var template = new AdaptiveCardTemplate(templateJson);
            var cardJson = template.Expand(new {
                Id = user.Id,
                Name = user.Name
            });
            var attachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };
            return MessageFactory.Attachment(attachment);
        }
    }
}
