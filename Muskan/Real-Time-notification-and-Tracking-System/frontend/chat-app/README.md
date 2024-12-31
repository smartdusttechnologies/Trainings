# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

### Code Splitting

This section has moved here: [https://facebook.github.io/create-react-app/docs/code-splitting](https://facebook.github.io/create-react-app/docs/code-splitting)

### Analyzing the Bundle Size

This section has moved here: [https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size](https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size)

### Making a Progressive Web App

This section has moved here: [https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app](https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app)

### Advanced Configuration

This section has moved here: [https://facebook.github.io/create-react-app/docs/advanced-configuration](https://facebook.github.io/create-react-app/docs/advanced-configuration)

### Deployment

This section has moved here: [https://facebook.github.io/create-react-app/docs/deployment](https://facebook.github.io/create-react-app/docs/deployment)

### `npm run build` fails to minify

This section has moved here: [https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify](https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify)






1. User Places Order
Data: UserId, RestaurantId, OrderId
Action: The user places an order with their details and the restaurant details.
Notification: A notification is sent to the restaurant indicating that there’s a new order.
2. Restaurant Accepts or Rejects the Order
Data: Restaurant decision (Accept/Reject)
Action: The restaurant either accepts or rejects the order.
Notification:
If accepted, a notification is sent to the user that the order has been accepted.
If rejected, a notification is sent to the user that the order has been rejected.
3. Restaurant Confirms Order Preparation
Data: Order preparation status (Confirmed)
Action: After preparing the order, the restaurant confirms it’s ready.
Notification:
A notification is sent to the user that the order is prepared.
Notifications are sent to riders within a 5km radius from the restaurant, indicating the availability of the order for delivery.
4. Rider Accepts the Delivery
Data: Rider accepts the order.
Action: The first rider who accepts the order is assigned the delivery.
Notification:
A notification is sent to the user, informing them that a rider has been assigned.
A notification is also sent to the rider, confirming the assignment.
5. Rider Sees Map with Locations
Data: Restaurant location, User location, Rider location.
Action:
The rider sees the map route from the restaurant to the user.
The user sees the rider's location on the map as the rider is en route.
The restaurant can also see the rider’s location on the map.
6. Rider Confirms Delivery Acceptance
Data: Delivery acceptance by rider.
Action: The rider confirms that they have accepted the order from the restaurant.
Notification:
The rider sends a notification to the user that the delivery has been accepted.
The rider also sees the map from the restaurant to the user.
The user sees the rider's location.
7. Rider Approaches User Location
Data: Rider proximity to user’s location.
Action:
As the rider gets closer to the user’s location, a warning is automatically sent to the user, indicating that the rider is arriving soon.
8. Order Successfully Delivered
Data: Delivery completion status.
Action:
After reaching the user’s location and completing the delivery, a notification is sent to both the user and the restaurant indicating that the order has been successfully delivered.