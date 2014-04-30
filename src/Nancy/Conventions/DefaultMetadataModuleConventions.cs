﻿namespace Nancy.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the default metadata module conventions.
    /// </summary>
    public class DefaultMetadataModuleConventions : IConvention
    {
        /// <summary>
        /// Initialise any conventions this class "owns".
        /// </summary>
        /// <param name="conventions">Convention object instance.</param>
        public void Initialise(NancyConventions conventions)
        {
            ConfigureMetadataModuleConventions(conventions);
        }

        /// <summary>
        /// Asserts that the conventions that this class "owns" are valid.
        /// </summary>
        /// <param name="conventions">Conventions object instance.</param>
        /// <returns>Tuple containing true/false for valid/invalid, and any error messages.</returns>
        public Tuple<bool, string> Validate(NancyConventions conventions)
        {
            if (conventions.MetadataModuleConventions == null)
            {
                return Tuple.Create(false, "The metadata module conventions cannot be null.");
            }

            return (conventions.MetadataModuleConventions.Count > 0) ?
                Tuple.Create(true, string.Empty) :
                Tuple.Create(false, "The metadata module conventions cannot be empty.");
        }

        private static void ConfigureMetadataModuleConventions(NancyConventions conventions)
        {
            conventions.MetadataModuleConventions = new List<Func<Type, IEnumerable<Type>, Type>>
                {
                    // 0 Handles: BlahModule -> BlahMetadataModule
                    (moduleType, metadataModuleTypes) =>
                        {
                            var moduleName = moduleType.FullName;
                            var i = moduleName.LastIndexOf("Module");
                            var metadataModuleName = moduleName.Substring(0, i) + "MetadataModule";

                            return
                                metadataModuleTypes.FirstOrDefault(
                                    m =>
                                    string.Compare(m.FullName, metadataModuleName, StringComparison.OrdinalIgnoreCase) == 0);
                        }
                };
        }
    }
}
