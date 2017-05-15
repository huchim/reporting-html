namespace Jaguar.Reporting.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using Mustache;

    internal sealed class IfEqualTagDefinition : ContentTagDefinition
    {
        private const string conditionParameter = "condition";
        private const string comparerParameter = "comparer";

        public IfEqualTagDefinition() : base("ifeq")
        {
        }

        /// <summary>
        /// Gets the tags that come into scope within the context of the current tag.
        /// </summary>
        /// <returns>The child tag definitions.</returns>
        protected override IEnumerable<string> GetChildTags()
        {
            return new string[] { "elif", "else" };
        }

        /// <summary>
        /// Gets the parameters that can be passed to the tag.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[]
            {
                new TagParameter(conditionParameter) { IsRequired = true },
                new TagParameter(comparerParameter) { IsRequired = true },
            };
        }

        /// <summary>
        /// Gets the parameters that are used to create a new child context.
        /// </summary>
        /// <returns>The parameters that are used to create a new child context.</returns>
        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new TagParameter[0];
        }

        /// <summary>
        /// Gets whether the given tag's generator should be used for a secondary (or substitute) text block.
        /// </summary>
        /// <param name="definition">The tag to inspect.</param>
        /// <returns>True if the tag's generator should be used as a secondary generator.</returns>
        public override bool ShouldCreateSecondaryGroup(TagDefinition definition)
        {
            return new string[] { "elif", "else" }.Contains(definition.Name);
        }

        /// <summary>
        /// Gets whether the primary generator group should be used to render the tag.
        /// </summary>
        /// <param name="arguments">The arguments passed to the tag.</param>
        /// <returns>
        /// True if the primary generator group should be used to render the tag;
        /// otherwise, false to use the secondary group.
        /// </returns>
        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            object condition = arguments[conditionParameter];
            object comparer = arguments[comparerParameter];
            return isConditionSatisfied(condition, comparer);
        }

        private bool isConditionSatisfied(object condition, object comparer)
        {
            return condition.Equals(comparer);
        }
    }
}