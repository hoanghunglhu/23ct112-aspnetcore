// Parallax hero background
const hero = document.querySelector('.hero');
window.addEventListener('scroll', () => {
    let offset = window.scrollY;
    hero.style.backgroundPositionY = offset * 0.3 + "px";
});

// Scroll reveal for cards
const cards = document.querySelectorAll('.card');

const revealCards = () => {
    const triggerBottom = window.innerHeight * 0.85;
    cards.forEach(card => {
        const cardTop = card.getBoundingClientRect().top;
        if(cardTop < triggerBottom){
            card.classList.add('show');
        }
    });
};

window.addEventListener('scroll', revealCards);
window.addEventListener('load', revealCards);

// Random tilt on hover for fun
cards.forEach(card => {
    card.addEventListener('mouseenter', () => {
        const rotate = (Math.random() - 0.5) * 15; // ±7.5 deg
        card.style.transform = `rotate(${rotate}deg) scale(1.05)`;
    });
    card.addEventListener('mouseleave', () => {
        card.style.transform = '';
    });
});
// Scroll reveal cards and about/blog
const revealElements = document.querySelectorAll('.card');

const reveal = () => {
    const triggerBottom = window.innerHeight * 0.85;
    revealElements.forEach(el => {
        const elTop = el.getBoundingClientRect().top;
        if(elTop < triggerBottom){
            el.classList.add('show');
        }
    });
};

window.addEventListener('scroll', reveal);
window.addEventListener('load', reveal);

// Animate skill bars
const skillBars = document.querySelectorAll('.progress');
skillBars.forEach(bar => {
    const width = bar.style.width;
    bar.style.width = 0;
    setTimeout(() => {
        bar.style.width = width;
    }, 500);
});

// Hover random tilt for cards
revealElements.forEach(card => {
    card.addEventListener('mouseenter', () => {
        const rotate = (Math.random() - 0.5) * 15;
        card.style.transform = `rotate(${rotate}deg) scale(1.05)`;
    });
    card.addEventListener('mouseleave', () => {
        card.style.transform = '';
    });
});
