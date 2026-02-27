# Health Management Web Application – Task List

## **Project Setup & Infrastructure**
1. Create ASP.NET MVC 8 project with .NET 8 runtime
2. Configure SQLite LocalDB and Entity Framework Core
3. Set up Azure OpenAI credentials and SDK connection
4. Configure project structure (Services, Repositories, Models folders)

## **Database & Data Layer**
5. Create User entity extending ASP.NET Identity with profile fields (age, weight, height, goal)
6. Create FoodLog entity and corresponding DbContext
7. Create ExerciseLog entity and corresponding DbContext
8. Implement Entity Framework migrations and initialize database
9. Create Repository pattern classes for FoodLog and ExerciseLog
10. Seed sample data for testing

## **Authentication & User Management**
11. Configure ASP.NET Identity with SQLite provider
12. Build user registration page with form validation
13. Build user login page with authentication
14. Create user profile edit page for personal data
15. Implement password security and account management

## **Food Intake Management**
16. Create FoodLog service class for business logic
17. Build MVC form for text-based food input
18. Implement image upload endpoint with file validation
19. Create Azure OpenAI text service to extract nutrients from food descriptions
20. Create Azure OpenAI Vision service to identify foods and nutrients from images
21. Build response parser to extract structured nutrition data (calories, macros, micronutrients)
22. Implement food log save functionality to SQLite
23. Create food log listing and history view

## **Exercise Management**
24. Create ExerciseLog service class
25. Build exercise input form (name, duration, intensity)
26. Implement calorie burn calculation formula based on weight, duration, exercise type
27. Implement exercise log save functionality
28. Create exercise history and log view

## **Dashboard & Reporting**
29. Build dashboard controller and view showing daily summary (intake vs. burned calories)
30. Create weekly trends page with nutritional data
31. Integrate Chart.js for calorie and nutrient visualizations
32. Implement AI insights generation using Azure OpenAI for personalized recommendations
33. Add data filtering (date range, exercise type, food category)

## **Security & Deployment**
34. Configure HTTPS enforcement
35. Implement CSRF anti-forgery token protection
36. Set up User Secrets for local development
37. Configure error handling and logging
38. Deploy to Azure App Service (optional final step)
