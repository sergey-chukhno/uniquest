/**
 * TROLLS ET PAILLETTES - Interactive Presentation
 * Compact Menu with Navigation
 */

// ==========================================
// INITIALIZATION
// ==========================================

document.addEventListener('DOMContentLoaded', () => {
    initializeAudioSystem();
    initializeParticles();
    initializeButtonEffects();
    
    console.log('🎮 Trolls et Paillettes - Présentation Chargée');
});

// ==========================================
// AUDIO SYSTEM
// ==========================================

let audioInitialized = false;
let musicPlaying = false;

function initializeAudioSystem() {
    const music = document.getElementById('backgroundMusic');
    const soundToggle = document.getElementById('soundToggle');
    const soundOn = soundToggle.querySelector('.sound-on');
    const soundOff = soundToggle.querySelector('.sound-off');
    
    if (!music || !soundToggle) return;
    
    // Set initial volume
    music.volume = 0.4;
    
    // Save music state before navigation
    window.addEventListener('beforeunload', () => {
        if (musicPlaying) {
            sessionStorage.setItem('musicPlaying', 'true');
            sessionStorage.setItem('musicTime', music.currentTime.toString());
        } else {
            sessionStorage.setItem('musicPlaying', 'false');
        }
    });
    
    // Try to autoplay (some browsers block this)
    const playPromise = music.play();
    
    if (playPromise !== undefined) {
        playPromise.then(() => {
            // Autoplay started
            audioInitialized = true;
            musicPlaying = true;
            soundToggle.classList.add('playing');
            soundOn.style.display = 'inline';
            soundOff.style.display = 'none';
            console.log('🎵 Musique démarrée automatiquement');
        }).catch(() => {
            // Autoplay blocked - user needs to click
            console.log('🎵 Cliquez sur le bouton son pour démarrer la musique');
        });
    }
    
    // Toggle button click handler
    soundToggle.addEventListener('click', () => {
        if (!audioInitialized) {
            // First click - initialize audio
            music.play().then(() => {
                audioInitialized = true;
                musicPlaying = true;
                soundToggle.classList.add('playing');
                soundOn.style.display = 'inline';
                soundOff.style.display = 'none';
                createSoundRipple(soundToggle);
                console.log('🎵 Musique démarrée');
            }).catch(error => {
                console.error('Erreur de lecture audio:', error);
            });
        } else {
            // Toggle play/pause
            if (musicPlaying) {
                music.pause();
                musicPlaying = false;
                soundToggle.classList.remove('playing');
                soundOn.style.display = 'none';
                soundOff.style.display = 'inline';
                console.log('🔇 Musique mise en pause');
            } else {
                music.play();
                musicPlaying = true;
                soundToggle.classList.add('playing');
                soundOn.style.display = 'inline';
                soundOff.style.display = 'none';
                console.log('🎵 Musique relancée');
            }
            createSoundRipple(soundToggle);
        }
    });
    
    // Add hover effect
    soundToggle.addEventListener('mouseenter', (e) => {
        createSoundHoverEffect(soundToggle, e);
    });
}

function createSoundRipple(button) {
    const ripple = document.createElement('div');
    
    ripple.style.position = 'absolute';
    ripple.style.left = '50%';
    ripple.style.top = '50%';
    ripple.style.width = '0';
    ripple.style.height = '0';
    ripple.style.borderRadius = '50%';
    ripple.style.background = 'rgba(212, 175, 55, 0.5)';
    ripple.style.transform = 'translate(-50%, -50%)';
    ripple.style.pointerEvents = 'none';
    ripple.style.transition = 'width 0.8s ease, height 0.8s ease, opacity 0.8s ease';
    ripple.style.opacity = '1';
    ripple.style.zIndex = '0';
    
    button.appendChild(ripple);
    
    setTimeout(() => {
        ripple.style.width = '200px';
        ripple.style.height = '200px';
        ripple.style.opacity = '0';
    }, 10);
    
    setTimeout(() => {
        ripple.remove();
    }, 800);
}

function createSoundHoverEffect(button, event) {
    const sparkle = document.createElement('div');
    const rect = button.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    
    sparkle.style.position = 'absolute';
    sparkle.style.left = x + 'px';
    sparkle.style.top = y + 'px';
    sparkle.style.width = '4px';
    sparkle.style.height = '4px';
    sparkle.style.borderRadius = '50%';
    sparkle.style.background = 'var(--primary-gold)';
    sparkle.style.transform = 'translate(-50%, -50%)';
    sparkle.style.pointerEvents = 'none';
    sparkle.style.boxShadow = '0 0 10px var(--primary-gold)';
    sparkle.style.animation = 'sparkleFloat 1s ease-out forwards';
    sparkle.style.zIndex = '2';
    
    button.appendChild(sparkle);
    
    setTimeout(() => {
        sparkle.remove();
    }, 1000);
}

// ==========================================
// PARTICLE SYSTEM
// ==========================================

function initializeParticles() {
    const particlesContainer = document.getElementById('particles');
    const particleCount = 25;
    
    for (let i = 0; i < particleCount; i++) {
        createParticle(particlesContainer);
    }
}

function createParticle(container) {
    const particle = document.createElement('div');
    particle.className = 'particle';
    
    // Random starting position
    particle.style.left = Math.random() * 100 + '%';
    
    // Random size
    const size = Math.random() * 2 + 1;
    particle.style.width = size + 'px';
    particle.style.height = size + 'px';
    
    // Random animation duration
    const duration = Math.random() * 15 + 10;
    particle.style.animationDuration = duration + 's';
    
    // Random delay
    const delay = Math.random() * 8;
    particle.style.animationDelay = delay + 's';
    
    container.appendChild(particle);
    
    // Recreate particle after animation ends
    setTimeout(() => {
        particle.remove();
        createParticle(container);
    }, (duration + delay) * 1000);
}

// ==========================================
// BUTTON EFFECTS
// ==========================================

function initializeButtonEffects() {
    const menuButtons = document.querySelectorAll('.menu-button');
    
    menuButtons.forEach(button => {
        // Enhanced hover effects
        button.addEventListener('mouseenter', (e) => {
            addHoverEffect(button, e);
        });
        
        button.addEventListener('mousemove', (e) => {
            updateHoverEffect(button, e);
        });
        
        button.addEventListener('mouseleave', () => {
            removeHoverEffect(button);
        });
        
        // Click ripple effect
        button.addEventListener('click', (e) => {
            createRipple(button, e);
        });
    });
}

function addHoverEffect(button, event) {
    // Create subtle ripple effect
    const ripple = document.createElement('div');
    const rect = button.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    
    ripple.style.position = 'absolute';
    ripple.style.left = x + 'px';
    ripple.style.top = y + 'px';
    ripple.style.width = '0';
    ripple.style.height = '0';
    ripple.style.borderRadius = '50%';
    ripple.style.background = 'rgba(212, 175, 55, 0.3)';
    ripple.style.transform = 'translate(-50%, -50%)';
    ripple.style.pointerEvents = 'none';
    ripple.style.transition = 'width 0.6s ease, height 0.6s ease, opacity 0.6s ease';
    ripple.style.opacity = '1';
    ripple.style.zIndex = '0';
    
    button.appendChild(ripple);
    
    // Animate ripple
    setTimeout(() => {
        ripple.style.width = '300px';
        ripple.style.height = '300px';
        ripple.style.opacity = '0';
    }, 10);
    
    // Remove ripple after animation
    setTimeout(() => {
        ripple.remove();
    }, 600);
}

function updateHoverEffect(button, event) {
    const rect = button.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    
    const centerX = rect.width / 2;
    const centerY = rect.height / 2;
    
    const percentX = (x - centerX) / centerX;
    const percentY = (y - centerY) / centerY;
    
    // Subtle 3D tilt effect
    const maxTilt = 5;
    const tiltX = percentY * maxTilt;
    const tiltY = -percentX * maxTilt;
    
    button.style.transform = `
        translateY(-5px) 
        scale(1.05) 
        perspective(1000px) 
        rotateX(${tiltX}deg) 
        rotateY(${tiltY}deg)
    `;
}

function removeHoverEffect(button) {
    button.style.transform = '';
}

function createRipple(button, event) {
    const ripple = document.createElement('div');
    const rect = button.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    
    ripple.style.position = 'absolute';
    ripple.style.left = x + 'px';
    ripple.style.top = y + 'px';
    ripple.style.width = '0';
    ripple.style.height = '0';
    ripple.style.borderRadius = '50%';
    ripple.style.background = 'rgba(255, 204, 102, 0.5)';
    ripple.style.transform = 'translate(-50%, -50%)';
    ripple.style.pointerEvents = 'none';
    ripple.style.transition = 'width 0.8s ease, height 0.8s ease, opacity 0.8s ease';
    ripple.style.opacity = '1';
    ripple.style.zIndex = '0';
    
    button.appendChild(ripple);
    
    setTimeout(() => {
        ripple.style.width = '400px';
        ripple.style.height = '400px';
        ripple.style.opacity = '0';
    }, 10);
    
    setTimeout(() => {
        ripple.remove();
    }, 800);
}

// ==========================================
// CONSOLE EASTER EGG
// ==========================================

console.log('%c⚔️ TROLLS ET PAILLETTES ✨', 
    'color: #d4af37; font-size: 28px; font-weight: bold; text-shadow: 0 0 10px #d4af37;'
);
console.log('%cRPG Unity Game - Présentation Interactive', 
    'color: #ffcc66; font-size: 16px; font-style: italic;'
);
console.log('%cPar Élodie, Louis & Sergey', 
    'color: #a0a0a0; font-size: 12px;'
);