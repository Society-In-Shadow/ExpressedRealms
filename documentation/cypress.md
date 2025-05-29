## Cypress
Cypress is configured in this repo.  Both the end to end and the component testing is implemented.
In order to run it on your local you need to go to the expressedRealms.client folder, and run one of the following commands.

For more inforamtion, see [Cypress](https://docs.cypress.io/guides/overview/why-cypress)

### Interactive GUI
When you run the below command, it will pop up a GUI that will allow you to run both types of tests and watch them run.
You can only do one or the other.  The main benefit here is that you can watch them run, and debug them in place to a degree.
The main benefit of using the GUI is that it will update in realtime, as you are modifying the components.

```shell
npm run open-cypress
```

### Component Testing
It does everything the GUI does, but headlessly.

```shell
npx run component-tests
```

### End to End Testing
The following runs the end to end testing via the command line.  It does everything the GUI does, it just does it headlessly.

```shell
npx cypress run
```
