﻿// <copyright file="Logging.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace AlgernonCommons
{
    using System;
    using System.Text;
    using CorrectAnimalNames;
    using UnityEngine;

    /// <summary>
    /// Logging utility class.
    /// </summary>
    public static class Logging
    {
        // Private fields.
        private static bool s_detailLogging = true;
        private static string s_modName;

        // Stringbuilder for messaging.
        private static StringBuilder s_message = new StringBuilder(128);

        /// <summary>
        /// Gets or sets a value indicating whether more detailed logging should be provided.
        /// If this is false, only errors, key messages, or exceptions will be logged (standard messages will be ignored).
        /// </summary>
        public static bool DetailLogging { get => s_detailLogging; set => s_detailLogging = value; }

        /// <summary>
        /// Gets the mod's logging name (to identify each line).
        /// </summary>
        private static string ModName
        {
            get
            {
                // Check for cached reference.
                if (s_modName == null)
                {
                    // No cached reference - if ModBase defines a name, use that, otherwise fall back to the assembly name.
                    s_modName = Mod.ModName ?? AssemblyUtils.Name;
                }

                return s_modName;
            }
        }

        /// <summary>
        /// Prints a single-line debugging message to the Unity output log with an "ERROR: " prefix, regardless of the 'detailed logging' setting.
        /// </summary>
        /// <param name="messages">Message to log (individual strings will be concatenated).</param>
        public static void Error(params object[] messages) => WriteMessage("ERROR: ", messages);

        /// <summary>
        /// Prints a single-line debugging message to the Unity output log, regardless of the 'detailed logging' setting.
        /// </summary>
        /// <param name="messages">Message to log (individual strings will be concatenated).</param>
        public static void KeyMessage(params object[] messages) => WriteMessage(string.Empty, messages);

        /// <summary>
        /// Prints a single-line debugging message to the Unity output log if the 'detailed logging' option is set (otherwise does nothing).
        /// </summary>
        /// <param name="messages">Message to log (individual strings will be concatenated).</param>
        public static void Message(params object[] messages)
        {
            if (s_detailLogging)
            {
                WriteMessage(string.Empty, messages);
            }
        }

        /// <summary>
        /// Prints an exception message to the Unity output log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="messages">Message to log (individual strings will be concatenated).</param>
        public static void LogException(Exception exception, params object[] messages)
        {
            // Use StringBuilder for efficiency since we're doing a lot of manipulation here.
            // Start with mod name (to easily identify relevant messages), followed by colon to indicate start of actual message.
            s_message.Length = 0;
            s_message.Append(ModName);
            s_message.Append(": ");

            // Add each message parameter.
            for (int i = 0; i < messages.Length; ++i)
            {
                s_message.Append(messages[i]);
            }

            // Finish with a new line and the exception information.
            s_message.AppendLine();
            s_message.AppendLine("Exception: ");
            s_message.AppendLine(exception.Message);
            s_message.AppendLine(exception.Source);
            s_message.AppendLine(exception.StackTrace);

            // Log inner exception as well, if there is one.
            if (exception.InnerException != null)
            {
                s_message.AppendLine("Inner exception:");
                s_message.AppendLine(exception.InnerException.Message);
                s_message.AppendLine(exception.InnerException.Source);
                s_message.AppendLine(exception.InnerException.StackTrace);
            }

            // Write to log.
            Debug.Log(s_message);
        }

        /// <summary>
        /// Prints a single-line debugging message to the Unity output log with a specified prefix.
        /// </summary>
        /// <param name="prefix">Prefix for message, if any.</param>
        /// <param name="messages">Message to log (individual strings will be concatenated).</param>
        private static void WriteMessage(string prefix, params object[] messages)
        {
            // Use StringBuilder for efficiency since we're doing a lot of manipulation here.
            // Start with mod name (to easily identify relevant messages), followed by colon to indicate start of actual message.
            s_message.Length = 0;
            s_message.Append(ModName);
            s_message.Append(": ");

            // Append prefix.
            s_message.Append(prefix);

            // Add each message parameter.
            for (int i = 0; i < messages.Length; ++i)
            {
                s_message.Append(messages[i]);
            }

            // Terminating period to confirm end of message.
            s_message.Append(".");

            Debug.Log(s_message);
        }
    }
}
