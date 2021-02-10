# SpaceBook
## Description
SpaceBook is a social media site for space nerds. SpaceBook allows you to see a space picture of the day, save pictures to your Favorites Gallery, comment on pictures, and post space pictures of your own. You can also meet, follow, and send messages to other space nerds on SpaceBook.

---
## Technologies
### Frontend:
* Marvel (UI Prototyping)
* Angular
* Bootstrap/CSS3
* Material Design

### Backend:
* ASP.NET Core Web API
* ASP.NET Core Identity (Authentication)
* Entity Framework Core (Code-first approach)
* Azure Database
* 3rd Party API: NASA APOD
  
### DevOps:
* Azure Pipelines
* SonarCloud
* Trello (Kanban)
  
---
## Features
* Users can view a home page with pictures of space without logging in.
* Users can click on pictures to view detailed descriptions and comments on pictures.
* Users can view profiles of other users and see pictures they have favorited.
* Users can register for an account with a unique username and a password.
* After registering, users can log in.
* Logged-in users can rate, favorite, and comment on pictures.
* Logged-in users can post pictures.
* Logged-in users can follow other users.
* Logged-in users can directly message other users.
* Logged-in users can see pictures they have favorited on their profile page.
* Logged-in users can can search for other users.
---
## Getting Started
* Open a command prompt in the directory where you want to create the project.
* Use the command `git clone https://github.com/12142020-dotnet-uta/P2_ATMC.git` to download the project.

### Backend
* Install Visual Studio to open the solution.
* Also install SQL server on your computer.
* Open SpaceBook-Backend/SpaceBook.sln in visual studio.
* Update the connection string in the ApplicationDbContext class in the SpaceBook.Repository Project.
* Open Visual Studio's Package Manager Console (PMC). In the PMC, run `update-database` to add the migrations to the database in your connection string.
* After you have a database, you can run the development version of the project using IIS.
* If you click the Play button at the top of the window, Visual Studio will build and run the backend with SwaggerUI showing all of the HTTP endpoints.

### Frontend
* Install node.js and npm [here](https://nodejs.org/en/)
* Open a command line terminal in the SpaceBook-Angular/ directory.
* In the command line, run `npm install`
* In the command line, run `npm start`
* Open an web browser and navigate to http://localhost:4200/

---
## Usage
Run both the frontend and the backend at the same time to use the website.
Select the register/login button in the top right to create an account.
After registering, you can log in.
After loggin in, you can access the full functionality of the site like saving, rating, and commenting on pictures.
After loggin in, you can also follow, message, and view profiles for any other users created in the database.

---
## Contributors
* Trevor Graham - Team Lead
* Alan Munoz - Frontend Lead
* Matt DeMonaco - Testing Lead
* Chris Sophiea - Backend Lead

All team members got experience with all parts of the program. Trevor and Chris started on the backend, and Alan and Matt started on the frontend. After one week, all team members switched roles to get more full stack experience.
  
---
## License
This project uses the following license: [MIT License](https://github.com/12142020-dotnet-uta/P2_ATMC/blob/main/LICENSE)


