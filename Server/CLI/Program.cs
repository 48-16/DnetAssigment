using System.Threading.Tasks;
using CLI.UI;                 
using FileRepositories;       
using RepositoryContracts;    

internal class Program
{
    private static async Task Main()
    {
        IUserRepository userRepo = new UserFileRepository();
        IPostRepository postRepo = new PostFileRepository();
        ICommentRepository commentRepo = new CommentFileRepository();
        
        var app = new CliApp(userRepo, postRepo, commentRepo);
        await app.StartAsync();
    }
}