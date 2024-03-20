
# Component Testing

## Using Parameters in Routes

If the component uses parameters from the route, all you need to do is the following.  Just make sure it follows the url
that you need.  I do not have suggestions on how to dynamically change this, as I've not run into that specific situation.
If I had to take a guess, you would need to create the custom router, and go from there.

```typescript
cy.mount(component, {
    pushRoute: '/objects/3'
});
```

## Customizing the Router

You can refer to the documentation on cypress in the following link.  You specifically want to look at the spec usage tab.
The component and Typings tabs have already been implemented.

[Cypress Vue Router Usage](https://docs.cypress.io/guides/component-testing/vue/examples#Vue-3)