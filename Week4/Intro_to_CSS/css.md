## CSS
- Cascading Style Sheet

### Ways to Style
- Inline
```html
<p style="color:green;">
```
- Internal
```html
<head>
    <style>
    h1 {
        color:red;
    }
    </style>
</head>
```
- External
```html
<head>
    <link rel="stylesheet" href="style.css">
</head>
```
```css
style.css
h1 {
        color:red;
}
```

### Selectors
- are a way to target one or more elements
- tag selectors
```css
h1 {
        color:red;
}
p {
    color:green;
}
```
- class selectors
```css
.MyClass {
        color:red;
}
```
- id selectors
```css
#MyID {
        color:red;
}
```
  
More advanced..
- group selector
```css
h1, h2, p {
        color:red;
}
```
- psuedo-class selectors
- psuedo-element
- sibling... and so on and so forth

### Rules
- define the styles we apply to the elements we've selected

### Box Model
![BoxModel](BoxModel.png)

### FlexBox, Grid
https://cssgridgarden.com/
https://flexboxfroggy.com/