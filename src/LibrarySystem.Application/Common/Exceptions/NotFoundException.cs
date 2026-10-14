namespace LibrarySystem.Application.Common.Exceptions;

public class NotFoundException(string resource, object key)
    : Exception($"{resource} with key '{key}' was not found.");
