﻿using System.Collections.Generic;
using Microsoft.TemplateEngine.Core.Contracts;
using Microsoft.TemplateEngine.Core.Operations;
using Newtonsoft.Json.Linq;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Config
{
    public static class ConditionalCustomConfig
    {
        public static List<IOperationProvider> ConfigureFromJObject(JObject rawConfiguration)
        {
            IReadOnlyList<string> ifToken = rawConfiguration.ArrayAsStrings("if");
            IReadOnlyList<string> elseToken = rawConfiguration.ArrayAsStrings("else");
            IReadOnlyList<string> elseIfToken = rawConfiguration.ArrayAsStrings("elseif");
            IReadOnlyList<string> actionableIfToken = rawConfiguration.ArrayAsStrings("actionableIf");
            IReadOnlyList<string> actionableElseToken = rawConfiguration.ArrayAsStrings("actionableElse");
            IReadOnlyList<string> actionableElseIfToken = rawConfiguration.ArrayAsStrings("actionableElseif");
            IReadOnlyList<string> actionsToken = rawConfiguration.ArrayAsStrings("actions");
            IReadOnlyList<string> endIfToken = rawConfiguration.ArrayAsStrings("endif");
            string id = rawConfiguration.ToString("id");
            bool trim = rawConfiguration.ToBool("trim");
            bool wholeLine = rawConfiguration.ToBool("wholeLine");

            string evaluatorName = rawConfiguration.ToString("evaluator");
            ConditionEvaluator evaluator = EvaluatorSelector.Select(evaluatorName);

            ConditionalTokens tokenVariants = new ConditionalTokens
            {
                IfTokens = ifToken,
                ElseTokens = elseToken,
                ElseIfTokens = elseIfToken,
                EndIfTokens = endIfToken,
                ActionableElseIfTokens = actionableElseIfToken,
                ActionableElseTokens = actionableElseToken,
                ActionableIfTokens = actionableIfToken,
                ActionableOperations = actionsToken
            };

            return new List<IOperationProvider>()
            {
                new Conditional(tokenVariants, wholeLine, trim, evaluator, id)
            };
        }
    }
}
