// <copyright file="PrivateSetterContractResolver.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Serialization
{
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Setter for private properties
    /// </summary>
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasSetter;
                    if (!hasSetter)
                    {
                        return null;
                    }
                }
            }

            return prop;
        }
    }
}