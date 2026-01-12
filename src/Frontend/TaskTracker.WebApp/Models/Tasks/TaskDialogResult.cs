using TaskTracker.WebApp.Models.Tasks;

public enum TaskDialogAction { Update, Delete }

public record TaskDialogResult(TaskDialogAction Action, TaskDetailsModel? Task = null);