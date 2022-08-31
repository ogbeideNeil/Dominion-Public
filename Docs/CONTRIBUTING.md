# Vismount-Dominion
## Code-Style
For this we will be using a Re-Sharper code style which we can mutually agree on, it has yet to be created.

## Branching
Every story that is being worked on gets its own branch, the branch will follow the name style of **{item id from scrumwise}-{story name}**

## Commit Style
The commit message for these branches will be as follows.
**{Name of story} - {name}**
All commits after the inital commit to a branch will be amends to the commit using `git commit --amend`
Before commiting your code to your branch, check for merges into the `Development` branch. If there has been a merge you will need to rebase your branch.
Do as follows:
```
git pull a fresh pull of your branch in a seperate folder
git checkout <branch> && git rebase <your-branch>
```
## Merging Code
Once your commit is pushed to your stories branch, you will create a pull request and have the code reviewed, once it has been reviewed and accepted by at least 2 team members
it can be merged to the `Development` branch

## After Review
Once your Pull Request has been reviewed and accepted by 2 or more members of the team, you will merge your branch into the `Development` branch
