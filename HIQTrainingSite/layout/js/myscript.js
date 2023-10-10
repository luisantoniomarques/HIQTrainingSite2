// top nav, toggle between hiding and showing the dropdown content 
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
    document.getElementById("myDropdown1").classList.remove("show");
    document.getElementById("myDropdown2").classList.remove("show");
    document.getElementById("myDropdown3").classList.remove("show");
    document.getElementById("myDropdown4").classList.remove("show");
}

// top nav, toggle between hiding and showing the dropdown content 
function myFunction1() {
    document.getElementById("myDropdown1").classList.toggle("show");
    document.getElementById("myDropdown").classList.remove("show");
    document.getElementById("myDropdown2").classList.remove("show");
    document.getElementById("myDropdown3").classList.remove("show");
    document.getElementById("myDropdown4").classList.remove("show");
}


// top nav, toggle between hiding and showing the dropdown content 
function myFunction2() {
    document.getElementById("myDropdown2").classList.toggle("show");
    document.getElementById("myDropdown").classList.remove("show");
    document.getElementById("myDropdown1").classList.remove("show");
    document.getElementById("myDropdown3").classList.remove("show");
    document.getElementById("myDropdown4").classList.remove("show");
}

// top nav, toggle between hiding and showing the dropdown content 
function myFunction3() {
    document.getElementById("myDropdown3").classList.toggle("show");
    document.getElementById("myDropdown").classList.remove("show");
    document.getElementById("myDropdown1").classList.remove("show");
    document.getElementById("myDropdown2").classList.remove("show");
    document.getElementById("myDropdown4").classList.remove("show");
}

// top nav, toggle between hiding and showing the dropdown content 
function myFunction4() {
    document.getElementById("myDropdown4").classList.toggle("show");
    document.getElementById("myDropdown").classList.remove("show");
    document.getElementById("myDropdown1").classList.remove("show");
    document.getElementById("myDropdown2").classList.remove("show");
    document.getElementById("myDropdown3").classList.remove("show");
}


function myFunctionAll(index) {

    switch (index) {
        case 0:
            myFunction();
            break;

        case 1:
            myFunction1();
            break;

        case 2:
            myFunction2();
            break;

        case 3:
            myFunction3();
            break;

        case 4:
            myFunction4();
            break;

        default:
            break;
    }


}




// Close the dropdown if the user clicks outside of it
window.onclick = function (e) {
    
    //if (!e.target.matches('.dropbtn')) {
    if (!$(event.target).hasClass('dropbtn')) {
        var myDropdown = document.getElementById("myDropdown");
        var myDropdown1 = document.getElementById("myDropdown1");
        var myDropdown2 = document.getElementById("myDropdown2");
        var myDropdown3 = document.getElementById("myDropdown3");
        var myDropdown4 = document.getElementById("myDropdown4");

        if (myDropdown.classList.contains('show')) {
            myDropdown.classList.remove('show');
        }
        else if (myDropdown1.classList.contains('show')) {
            myDropdown1.classList.remove('show');
        }
        else if (myDropdown2.classList.contains('show')) {
            myDropdown2.classList.remove('show');
        }
        else if (myDropdown3.classList.contains('show')) {
            myDropdown3.classList.remove('show');
        }
        else if (myDropdown4.classList.contains('show')) {
            myDropdown4.classList.remove('show');
        }    
    }
}


// Open and close sidenav
function openNav() {
    document.getElementById("mySidenav").style.width = "100%";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}



//modal effects

var ModalEffects = (function() {

    function init() {

        var overlay = document.querySelector('.md-overlay');

        [].slice.call(document.querySelectorAll('.md-trigger')).forEach(function(el, i) {

            var modal = document.querySelector('#' + el.getAttribute('data-modal')),
                close = modal.querySelector('.md-close');

            function removeModal(hasPerspective) {
                classie.remove(modal, 'md-show');

                if (hasPerspective) {
                    classie.remove(document.documentElement, 'md-perspective');
                }
            }

            function removeModalHandler() {
                removeModal(classie.has(el, 'md-setperspective'));
            }

            el.addEventListener('click', function(ev) {
                classie.add(modal, 'md-show');
                overlay.removeEventListener('click', removeModalHandler);
                overlay.addEventListener('click', removeModalHandler);

                if (classie.has(el, 'md-setperspective')) {
                    setTimeout(function() {
                        classie.add(document.documentElement, 'md-perspective');
                    }, 25);
                }
            });

            close.addEventListener('click', function(ev) {
                ev.stopPropagation();
                removeModalHandler();
            });

        });

    }

    init();

})();