namespace Kotoba.Core.Manager;

public interface ITranslationManager
{
    //TODO: I really dislike this naming
    Task FetchAndPersistAsync();
}