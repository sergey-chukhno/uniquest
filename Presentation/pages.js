/**
 * TROLLS ET PAILLETTES - Detail Pages
 * Navigation and Animations
 */

// ==========================================
// INITIALIZATION
// ==========================================

document.addEventListener('DOMContentLoaded', () => {
    initializeAudioSystem();
    initializeParticles();
    initializeNavButtons();
    initializeAnimations();
    
    console.log('🎮 Page de détails chargée');
});

// ==========================================
// AUDIO SYSTEM
// ==========================================

let audioInitialized = false;
let musicPlaying = false;

function initializeAudioSystem() {
    const music = document.getElementById('backgroundMusic');
    const soundToggle = document.getElementById('soundToggle');
    
    if (!music || !soundToggle) return;
    
    const soundOn = soundToggle.querySelector('.sound-on');
    const soundOff = soundToggle.querySelector('.sound-off');
    
    // Set initial volume
    music.volume = 0.4;
    
    // Check if music was playing on previous page (from sessionStorage)
    const wasMusicPlaying = sessionStorage.getItem('musicPlaying') === 'true';
    
    if (wasMusicPlaying) {
        // Continue playing from where we left off
        const savedTime = parseFloat(sessionStorage.getItem('musicTime') || '0');
        music.currentTime = savedTime;
        music.play().then(() => {
            audioInitialized = true;
            musicPlaying = true;
            soundToggle.classList.add('playing');
            soundOn.style.display = 'inline';
            soundOff.style.display = 'none';
            console.log('🎵 Musique continuée');
        }).catch(() => {
            console.log('🎵 Erreur de lecture');
        });
    } else {
        // Try to autoplay for first time visitors
        const playPromise = music.play();
        
        if (playPromise !== undefined) {
            playPromise.then(() => {
                audioInitialized = true;
                musicPlaying = true;
                soundToggle.classList.add('playing');
                soundOn.style.display = 'inline';
                soundOff.style.display = 'none';
                sessionStorage.setItem('musicPlaying', 'true');
                console.log('🎵 Musique démarrée');
            }).catch(() => {
                console.log('🎵 Cliquez pour démarrer');
            });
        }
    }
    
    // Save music state before page unload
    window.addEventListener('beforeunload', () => {
        if (musicPlaying) {
            sessionStorage.setItem('musicPlaying', 'true');
            sessionStorage.setItem('musicTime', music.currentTime.toString());
        } else {
            sessionStorage.setItem('musicPlaying', 'false');
        }
    });
    
    // Toggle button click handler
    soundToggle.addEventListener('click', () => {
        if (!audioInitialized) {
            music.play().then(() => {
                audioInitialized = true;
                musicPlaying = true;
                soundToggle.classList.add('playing');
                soundOn.style.display = 'inline';
                soundOff.style.display = 'none';
                sessionStorage.setItem('musicPlaying', 'true');
                createSoundRipple(soundToggle);
                console.log('🎵 Musique démarrée');
            }).catch(error => {
                console.error('Erreur:', error);
            });
        } else {
            if (musicPlaying) {
                music.pause();
                musicPlaying = false;
                soundToggle.classList.remove('playing');
                soundOn.style.display = 'none';
                soundOff.style.display = 'inline';
                sessionStorage.setItem('musicPlaying', 'false');
                console.log('🔇 Pause');
            } else {
                music.play();
                musicPlaying = true;
                soundToggle.classList.add('playing');
                soundOn.style.display = 'inline';
                soundOff.style.display = 'none';
                sessionStorage.setItem('musicPlaying', 'true');
                console.log('🎵 Reprise');
            }
            createSoundRipple(soundToggle);
        }
    });
    
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
    if (!particlesContainer) return;
    
    const particleCount = 20;
    
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
// NAVIGATION BUTTONS
// ==========================================

function initializeNavButtons() {
    const navButtons = document.querySelectorAll('.nav-button');
    
    navButtons.forEach(button => {
        button.addEventListener('mouseenter', (e) => {
            createButtonRipple(button, e);
        });
        
        button.addEventListener('click', (e) => {
            createClickEffect(button, e);
        });
    });
}

function createButtonRipple(button, event) {
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
    
    button.style.position = 'relative';
    button.style.overflow = 'hidden';
    button.appendChild(ripple);
    
    setTimeout(() => {
        ripple.style.width = '200px';
        ripple.style.height = '200px';
        ripple.style.opacity = '0';
    }, 10);
    
    setTimeout(() => {
        ripple.remove();
    }, 600);
}

function createClickEffect(button, event) {
    const effect = document.createElement('div');
    const rect = button.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;
    
    effect.style.position = 'absolute';
    effect.style.left = x + 'px';
    effect.style.top = y + 'px';
    effect.style.width = '0';
    effect.style.height = '0';
    effect.style.borderRadius = '50%';
    effect.style.background = 'rgba(255, 204, 102, 0.6)';
    effect.style.transform = 'translate(-50%, -50%)';
    effect.style.pointerEvents = 'none';
    effect.style.transition = 'width 0.5s ease, height 0.5s ease, opacity 0.5s ease';
    effect.style.opacity = '1';
    effect.style.zIndex = '1';
    
    button.appendChild(effect);
    
    setTimeout(() => {
        effect.style.width = '150px';
        effect.style.height = '150px';
        effect.style.opacity = '0';
    }, 10);
    
    setTimeout(() => {
        effect.remove();
    }, 500);
}

// ==========================================
// SCROLL ANIMATIONS
// ==========================================

function initializeAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.animation = 'fadeInUp 0.6s ease-out';
                entry.target.style.opacity = '1';
            }
        });
    }, observerOptions);
    
    // Observe content sections
    document.querySelectorAll('.content-section, .team-card, .design-card, .feature-item').forEach(element => {
        observer.observe(element);
    });
}

// ==========================================
// KEYBOARD NAVIGATION
// ==========================================

document.addEventListener('keydown', (e) => {
    const navButtons = Array.from(document.querySelectorAll('.nav-button'));
    
    switch(e.key) {
        case 'Escape':
            // Go back to home
            const homeButton = navButtons.find(btn => btn.getAttribute('href') === 'index.html');
            if (homeButton) {
                window.location.href = 'index.html';
            }
            break;
            
        case 'ArrowLeft':
            // Previous page
            const prevButton = navButtons.find(btn => btn.textContent.includes('Précédent'));
            if (prevButton) {
                e.preventDefault();
                window.location.href = prevButton.getAttribute('href');
            }
            break;
            
        case 'ArrowRight':
            // Next page
            const nextButton = navButtons.find(btn => btn.textContent.includes('Suivant'));
            if (nextButton) {
                e.preventDefault();
                window.location.href = nextButton.getAttribute('href');
            }
            break;
    }
});

// ==========================================
// SMOOTH SCROLL
// ==========================================

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// ==========================================
// CONSOLE MESSAGE
// ==========================================

console.log('%c⚔️ TROLLS ET PAILLETTES ✨', 
    'color: #d4af37; font-size: 24px; font-weight: bold; text-shadow: 0 0 10px #d4af37;'
);
console.log('%cNavigation: ← Précédent | → Suivant | Esc Accueil', 
    'color: #ffcc66; font-size: 12px;'
);
