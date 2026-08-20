# Tooltips
From time to time, there will be a need to have a tool tip in the tool, here's what you need:



```html
<span
    v-if="props.modifier.notes"
    v-tooltip.bottom="props.modifier.notes"
    :title="props.modifier.notes"
    class="material-symbols-outlined inline-icon"
>
    info
</span>

```

## .inline-icon
This is a global utility class in the overrides.css file, this allows the icon to be centered with the rest of the text