using Keepi.Shared;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> AddNewPost(string Category, string Text, Guid UserId);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostsByUserId(Guid _UserId);
        Task<List<bool>> DeletePost(Guid _PostId);
        Task<List<Post>> EditPostCategory(Guid _PostId, string _newCategory);
        Task<List<Post>> EditPostContent(Guid _PostId, string _newContent);
        Task<List<bool>> SavePostToUserCollection(Guid _UserId, Guid _PostId);
        Task<List<bool>> UnsavePostFromUserCollection(Guid _UserId, Guid _PostId);
        Task<List<Post>> GetUserSavedPosts(Guid _UserId);

        //Task<List<bool>> Test();
    }
}
