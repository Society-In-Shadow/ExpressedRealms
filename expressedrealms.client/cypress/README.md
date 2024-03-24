
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

## Testing Toast Popups

You won't see the popups happen in the component tests, it won't load them in.  Instead you need to spy on the toaster,
then treat it like an intercept call.

It's actually pretty straight forward.  You want to add one or more of the following lines before the mount command.

```typescript
cy.spy(toasters, 'success').as("toasterSuccess");
cy.spy(toasters, 'error').as("toasterError");
cy.spy(toasters, 'warning').as("toasterWarning");
cy.spy(toasters, 'info').as("toasterInfo");
cy.spy(toasters, 'secondary').as("toasterSecondary");
cy.spy(toasters, 'contrast').as("toasterContrast");
```

Then for the actual testing (Tweak as needed)
```typescript
cy.get('@toasterSuccess').should('have.been.calledWith', 'Title', 'Message');
cy.get('@toasterSuccess').should('have.been.calledWith', 'Message');
```
