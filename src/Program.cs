using Octokit;

namespace Example
{
    partial class Program
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? throw new ArgumentNullException(variableName, "Environment variable not found.");
        }

        static void Main(string[] args)
        {
            // Get values from the environment variables
            string _outputFile = GetEnvironmentVariable("GITHUB_OUTPUT");
            string _owner = GetEnvironmentVariable("_owner");
            string _repo = GetEnvironmentVariable("_repo");
            string _token = GetEnvironmentVariable("_token");

            // Create a github client to interact with repositories
            GitHubClient client = new GitHubClient(new ProductHeaderValue("Example"));
            client.Credentials = new Credentials(_token);

            // Get a repository
            Repository repo = Task.Run(() => client.Repository.Get(_owner, _repo)).Result;

            // Get repository branches
            var branches = Task.Run(() => client.Repository.Branch.GetAll(repo.Id)).Result;

            if (branches.Count == 0)
            {
                Console.WriteLine($"There are no branches on the repository.");
            }
            else
            {
                Console.WriteLine($"Found {branches.Count} branches:");
                foreach (var branch in branches) 
                    Console.WriteLine(branch.Name);
            }

            // Set the output variable "branch_count" to branches.Count
            // This output variable then can be used from other actions.
            File.WriteAllText(_outputFile, $"branch_count={branches.Count}");
        }
    }
}
