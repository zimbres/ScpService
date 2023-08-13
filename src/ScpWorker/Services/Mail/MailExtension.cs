namespace ScpWorker.Services.Mail;

static class MailExtension
{
    internal static bool IsNotNullOrEmpty<T>(this IEnumerable<T> value)
    {
        return value != null && value.Any();
    }
    internal static bool IsNotNullOrEmptyOrWhiteSpace(this string value)
    {
        return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
    }
    internal static bool IsNotNull(this object value)
    {
        return value != null;
    }
}
