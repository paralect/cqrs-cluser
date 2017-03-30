// <copyright file="ReflectionUtils.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Snapshoting
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Reflection utility
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Returns attribute instance for specified type. Will return default type value if not found or not single.
        /// </summary>
        /// <param name="type">Target type</param>
        /// <typeparam name="TAttribute">type of searcing atribute</typeparam>
        /// <returns>Returns attribute instance for specified type.</returns>
        public static TAttribute GetSingleAttribute<TAttribute>(MemberInfo type) where TAttribute : class
        {
            var identities = type.GetCustomAttributes(typeof(TAttribute), false).ToArray();

            if (identities.Length != 1)
            {
                return default(TAttribute);
            }

            return identities[0] as TAttribute;
        }
    }
}
