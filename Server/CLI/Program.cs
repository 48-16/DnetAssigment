using CLI.UI;
using InMemoryRepositories;

var userRepo = new UserInMemoryRepository();
var postRepo = new PostInMemoryRepository();
var commentRepo = new CommentInMemoryRepository();

var app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();