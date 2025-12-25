using TaskTracker.WebApp.Models;

public enum TaskDialogAction { Update, Delete }

public record TaskDialogResult(TaskDialogAction Action, TaskDetailsModel? Task = null);