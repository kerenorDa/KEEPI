using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Keepi.Client.Repositories.Implementation
{
    public class PostRepository : Repository, IPostRepository
    {
        private const string URL = "api/Post";

        private readonly HttpClient _httpClient;
        public PostRepository(HttpClient httpClient, IHttpService commService) : base(commService)
        {
            _httpClient = httpClient;
        }

        //public async Task<List<bool>> Test() =>
        //     await Get<bool>(URL + System.IO.Path.AltDirectorySeparatorChar + "test");

        public async Task<List<Post>> AddNewPost(string Category, string Text, Guid UserId) =>
            await Get<Post>($"api/Post/newPost/{Category}/{Text}/{UserId}");

         public async Task<List<Post>> GetAllPosts() =>
            await Get<Post>($"api/Post/getAllPosts");

         public async Task<List<Post>> GetPostsByUserId(Guid _UserId) =>
            await Get<Post>($"api/Post/getUserPosts/{_UserId}");

        public async Task<List<bool>> DeletePost(Guid _PostId) =>
            await Get<bool>($"api/Post/deletePost/{_PostId}");

        public async Task<List<Post>> EditPostCategory(Guid _PostId, string _newCategory) =>
            await Get<Post>($"api/Post/editPostCategory/{_PostId}/{_newCategory}");

         public async Task<List<Post>> EditPostContent(Guid _PostId, string _newContent) =>
            await Get<Post>($"api/Post/editPostContent/{_PostId}/{_newContent}");
        
        public async Task<List<bool>> SavePostToUserCollection(Guid _UserId, Guid _PostId) =>
            await Get<bool>($"api/Post/savePostToUserCollection/{_UserId}/{_PostId}");

        public async Task<List<bool>> UnsavePostFromUserCollection(Guid _UserId, Guid _PostId) =>
            await Get<bool>($"api/Post/unsavePostFromUserCollection/{_UserId}/{_PostId}");

         public async Task<List<Post>> GetUserSavedPosts(Guid _UserId) =>
            await Get<Post>($"api/Post/getUserSavedPosts/{_UserId}");

    }

}
