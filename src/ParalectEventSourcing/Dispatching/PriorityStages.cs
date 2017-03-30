// <copyright file="PriorityStages.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    /// <summary>
    /// Stages are conventions. You are responsible for doing operations that are
    /// sutable for particular stage.
    /// We do not guarantee any order of handlers that belong to one stage. They will run in parallel.
    /// But we guarantee strict order for handlers in different stages.
    /// </summary>
    public static class PriorityStages
    {
        /// <summary>
        /// Command handling stage, before
        /// </summary>
        public const int CommandHandlingBefore = 1;

        /// <summary>
        /// Command handling stage, before
        /// </summary>
        public const int CommandHandling = 2;

        /// <summary>
        /// Command handling stage, before
        /// </summary>
        public const int CommandHandlingAfter = 3;

        /// <summary>
        /// Handlers in this stage know that document handlers wasn't started yet.
        /// I do not see use case for this stage for our current needs, added just
        /// for symmetry with ViewHandling_Completed.
        /// </summary>
        public const int ViewHandlingStarted = 8;

        /// <summary>
        /// Let's use this stage for events preprocessing. In this stage you know exactly, that
        /// primary DocumentHandling stage is not yet started.
        /// For example, we can handle events like Student_DeletedEvent, knowing that Student
        /// document wasn't deleted yet. View will be deleted in ViewHandling stage.
        /// </summary>
        public const int ViewHandlingBefore = 9;

        /// <summary>
        /// Let's use this stage for the most of operations.
        /// This stage should be default for all View Handlers (put [Priority(PriorityStages.ViewHandling)] on every handler in ViewHandlers folder).
        /// </summary>
        public const int ViewHandling = 10;

        /// <summary>
        /// When this stage will run, all documents should be mostly updated (or deleted). In this stage you can
        /// do post processing - access documents that were modified in previous stages. For example,
        /// if you are handling Student_NameChangedEvent on this stage you can be sure that Student document
        /// was updated in previous (ViewHandling) stage.
        /// </summary>
        public const int ViewHandlingAfter = 11;

        /// <summary>
        /// Handlers in this stage know that document handlers fully completed and all needed documents updated.
        /// Most handlers that access document database should run in this stage. For example, our notifications
        /// handlers should have access to fully updated documents in order to prepare mail message.
        /// </summary>
        public const int ViewHandlingCompleted = 12;
    }
}