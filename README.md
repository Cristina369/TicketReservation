# Description
The Ticket Reservation Application offers a platform for users to discover upcoming concerts, explore event details, select seats via interactive SVG seat maps, and securely complete bookings.
Administrators manage tickets, seats, events, and reservations for efficient operations.

![Home page](https://github.com/Cristina369/TicketReservation/blob/main/TicketReservation.UI/src/app/shared/components/Ticket-Reservation.jpg?raw=true "Ticket Reservation Application")

Admins manage all aspects of ticket management, including tickets, seats, events, and reservations, through an intuitive dashboard designed for efficiency and ease of use.
</br>

## Technology Stack
#### Backend:
- Programming Language: C#
- Framework: ASP.NET Core
- Data Access: Entity Framework Core
- Database: SQL Server
- Authentication and Authorization: ASP.NET Identity, JWT (JSON Web Tokens)
- API: ASP.NET Core Web API
#### Frontend:
- Programming Language: TypeScript
- Framework: Angular
- UI Library: Bootstrap
- State Management: RxJS
- Routing: Angular Router

## Demo
coming soon...
</br>

## Features
- Event Discovery: Users can explore upcoming concerts and view detailed event information.
- Seat Selection: Interactive SVG seating charts enable users to select specific seats for reservations.
- Booking Management: Users can book seats for desired events and manage their reservations.
- Admin Dashboard: Admins have full control over ticket management, including tickets, seats, events, and reservations.
- Secure Authentication and Authorization: The app ensures robust security with ASP.NET Identity and JWT.
- Responsive Design: The frontend is built with Angular and Bootstrap for a seamless user experience across devices.
</br>

## Getting Started
### Prerequisits
Before running the application, ensure you have the following installed:
- Node.js: To run the frontend Angular application.
- Angular CLI: To build and serve the Angular application.
- .NET Core SDK: To run the backend ASP.NET Core application.
- SQL Server: To host the database .

#### Clone the Repository
```git clone https://github.com/your-username/car-rental-app.git```
```cd car-rental-app```

#### Frontend Setup
```cd frontend```
```npm install```
```ng serve```

#### Backend Setup
```cd backend```
```dotnet restore```   
```dotnet run```

#### Database Configuration
Update the database connection string in appsettings.json in the backend project if needed.
Run Entity Framework Core migrations to apply database changes:
```dotnet ef database update```

#### Login Credentials
Use the following credentials for testing:
- Admin: Username: superadmin@blog.com | Password: Superadmin@123
- User: Username: User1@gmail.com | Password: User123&.

#### Notes
Ensure both frontend and backend servers are running simultaneously for full application functionality.
For production deployment, update environment variables and configure appropriate security settings.
</br>

### Additional Details
The application utilizes SVG for interactive seat maps, enabling intuitive management of ticket and seat selections for concert events.
