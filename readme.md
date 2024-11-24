# Repository Contribution Guidelines

To maintain consistency and ensure a smooth workflow in this repository, please follow these rules:

---

## General Rules:
1. **No direct pushes to the `main` branch.**
    - The `main` branch is protected. Changes must be submitted via Pull Requests (PRs) and approved before merging.

2. **Work on tasks in separate branches.**
    - Each task or feature should be implemented in a new branch based on the `dev` branch.
    - Use the proper naming convention for branches (see below).

3. **Branch Naming Convention:**
    - Use the following prefixes for branch names:
        - `feat-...` – for new features.
        - `bugfix-...` – for fixing bugs.
        - `hotfix-...` – for urgent fixes.
        - `refactor-...` – for code refactoring.
        - `test-...` – for changes to or creation of tests.
        - `docs-...` – for updates to documentation.

4. **Pull Requests:**
    - All changes must be submitted as Pull Requests to `dev` or `main`.
    - PRs should have descriptive titles and include a summary of changes.
    - PRs require at least one approval before merging.
    - Resolving all code review comments is mandatory.

5. **Commit Messages: \**Optional**
    - Commit messages should be clear and concise. Use prefixes like:
        - `feat: ...`
        - `fix: ...`
        - `docs: ...`
        - `refactor: ...`

---

## Workflow for Completing Tasks:
1. **Start the Task:**
    - Ensure you are on the `dev` branch:
      ```bash
      git checkout dev
      ```
    - Create a new branch:
      ```bash
      git checkout -b feat-your-feature-name
      ```

2. **Implement the Task:**
    - Write the code for your task.
    - Regularly commit your changes:
      ```bash
      git add .
      git commit -m "feat: added new feature"
      ```

3. **Push and Create a Pull Request:**
    - Push your branch to the remote repository:
      ```bash
      git push origin feat-your-feature-name
      ```
    - Open a Pull Request to the `dev` branch.
    - Add a detailed description of your changes.

4. **Code Review:**
    - Wait for your code to be reviewed and approved.
    - If changes are requested, make them in the same branch and push the updates:
      ```bash
      git add .
      git commit -m "fix: addressed review comments"
      git push
      ```

5. **Merge:**
    - Once the Pull Request is approved, it can be merged into the `dev` branch.

---

## Additional Notes:
- **Follow coding standards:** Ensure your code adheres to the established style and conventions.
- **Respect the structure:** Do not modify the project structure without prior discussion.

---

By following these guidelines, we can maintain a clean codebase and streamline collaboration. If you have any questions, feel free to contact the repository administrator.
