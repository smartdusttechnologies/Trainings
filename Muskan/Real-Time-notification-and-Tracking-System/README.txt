# ASP.NET Signalr Project  

## Prerequisites

Make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) version 6.0 or higher.
- [Visual Studio](https://visualstudio.microsoft.com/) with ASP.NET and Web Development workload enabled.
- [Node](.js runtime) version 19 or higher.

## Getting Started

1. Clone the repository:

    git clone https://github.com/smartdusttechnologies/Trainings.git    

2. Navigate to the project folder:
    
  **  For backend (asp.net core)  **

    cd Real-Time-Notification-And-Tracking-App/Backend/Signal_R.sln
  
3. Restore the project dependencies:

    dotnet restore

4. Run the project:
  
    dotnet run
   
  **  For Frontend (react app)  ** 

    cd Real-Time-Notification-And-Tracking-App/frontend/chat-app

   Install the required Node packages:

   npm install

   npm start


How to test the project ?? 

So the app run and default open in localhost:3000

There are four tab to test the project
1. Place Order

   Navigate to: localhost:3000/user/place-order
   Fill in the required input fields like Order ID, Restaurant ID, and User ID.

  Important Note:

    Use User ID: 456 to receive notifications at localhost:3000/user/notifications.
    Use Restaurant ID: 789 to receive notifications at localhost:3000/restaurant/notifications.
    If you use other IDs, notifications will not appear.

2. Restaurant Tab

   Navigate to: localhost:3000/restaurant/notifications
   View notifications for upcoming orders.
   Accept or reject orders.
   After preparing an order, notify the user by confirming the preparation.

3. User Notification Tab

   Navigate to: localhost:3000/user/notifications
   View notifications related to your orders.

4. Rider Tab

   Navigate to: localhost:3000/rider/notifications
   View notifications for upcoming orders.
   Accept or reject orders.
   Access map navigation for delivery tracking.





