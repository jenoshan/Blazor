
function myFunction(x) {
    var y = document.getElementById('sitid').innerHTML;
    if (x.matches) { // If media query matches
        if (y!==null) {
y.add('collapse');
        }
        
    } else {
        if (y !== null) {
            y.remove('collapse');
        }
    }
}
var x = window.matchMedia("(max-width: 768px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) 