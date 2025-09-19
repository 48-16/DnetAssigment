using RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace CLI.UI
{
    public class CliApp
    {
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;
        private readonly ICommentRepository _commentRepo;

        public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
        {
            _userRepo = userRepo;
            _postRepo = postRepo;
            _commentRepo = commentRepo;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Create user");
                Console.WriteLine("2. List posts");
                Console.WriteLine("3. Create post");
                Console.WriteLine("4. Add comment");
                Console.WriteLine("5. View post");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateUserAsync();
                        break;
                    case "2":
                        await ListPostsAsync();
                        break;
                    case "0":
                        return;
                    case "3":
                        await CreatePostAsync();
                        break;
                    case "4":
                        await AddCommentsAsync();
                        break;
                    case "5":
                        await ViewPostsAsync();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
        private async Task ViewPostsAsync()
        {
            Console.WriteLine("Enter post id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id");
                return;
            }
            var post = await _postRepo.GetSingleAsync(id);
            if (post == null)
            {
                Console.WriteLine("Post not found");
                return;
            }
            Console.WriteLine($"Title: {post.Title}\nBody: {post.Body}");
            var comments = _commentRepo.GetManyAsync().Where(c => c.PostId == post.Id);
            Console.WriteLine("\nComments:");
            foreach (var c in comments)
            {
                Console.WriteLine($"- ({c.UserId}) {c.Body}");
            }
        }

        private async Task CreateUserAsync()
        {
            Console.Write("Enter username: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            string pass = Console.ReadLine() ?? "";

            var user = new Entities.User { Username = name, Password = pass };
            await _userRepo.AddAsync(user);
            Console.WriteLine($"User created with id: {user.Id}");
        }

        private async Task ListPostsAsync()
        {
            var posts =  _postRepo.GetManyAsync();
            foreach (var p in posts)
            {
                Console.WriteLine($"[{p.Id}] {p.Title}");
            }
        }

        private async Task CreatePostAsync()
        {
            Console.Write("Enter user id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id");
                return;
            }
            Console.Write("Enter post title: ");
            string title = Console.ReadLine() ?? "";
            
            Console.Write("Enter post body: ");
            string body = Console.ReadLine() ?? "";
            
            var post = new Entities.Post
            {
                UserId = id,
                Title = title,
                Body = body
            };
            await _postRepo.AddAsync(post);
            Console.WriteLine($"Post created with id: {post.Id}");
        }

        private async Task AddCommentsAsync()
        {
            Console.Write("Enter post id: ");
            if (!int.TryParse(Console.ReadLine(), out int postId))
            {
                Console.WriteLine("Invalid id");
                return;
            }
            Console.Write("Enter user id: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid id");
                return;
            }
            Console.Write("Enter comment : ");
            string body = Console.ReadLine() ?? "";
            
            var comment = new Entities.Comment
            {
                PostId = postId,
                UserId = userId,
                Body = body
            };
            await _commentRepo.AddAsync(comment);
            Console.WriteLine($"Comment created with id: {comment.Id}");
        }
    }
}