export function addRunCommonRequiredTests(form: any) {
  return {
    ...form,
    runCommonRequiredTests(key: string, label: string, validValue: any = '123') {
      it('Label is correct', async () => {
        expect(form.fields[key].label).toEqual(label)
      })

      it('Is Required', async () => {
        form.fields[key].field.value = null
        await form.handleSubmit(() => {})()
        expect(form.fields[key].error.value).toEqual(`${label} is a required field`)
      })

      it('No Errors when it\'s a valid value', async () => {
        form.fields[key].field.value = validValue
        await form.handleSubmit(() => {})()
        expect(form.fields[key].error?.value).toBeUndefined()
      })
    },
  }
}
