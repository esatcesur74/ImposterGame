document.addEventListener('DOMContentLoaded', function () {
    // Remote Play button functionality
    const remotePlayBtn = document.getElementById('remotePlayBtn');
    const remotePlayForm = document.getElementById('remotePlayForm');
    const createRemoteBtn = document.getElementById('createRemoteBtn');
    const createRemoteForm = document.getElementById('createRemoteForm');

    if (remotePlayBtn && remotePlayForm) {
        remotePlayBtn.addEventListener('click', function () {
            // Toggle display of remote play form
            if (remotePlayForm.style.display === 'none') {
                remotePlayForm.style.display = 'block';
                if (createRemoteForm) createRemoteForm.style.display = 'none';
            } else {
                remotePlayForm.style.display = 'none';
            }
        });
    }

    if (createRemoteBtn && createRemoteForm) {
        createRemoteBtn.addEventListener('click', function () {
            // Toggle display of create remote form
            if (createRemoteForm.style.display === 'none') {
                createRemoteForm.style.display = 'block';
                if (remotePlayForm) remotePlayForm.style.display = 'none';
            } else {
                createRemoteForm.style.display = 'none';
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    // Remote Play button functionality
    const remotePlayBtn = document.getElementById('remotePlayBtn');
    const remotePlayForm = document.getElementById('remotePlayForm');
    const createRemoteBtn = document.getElementById('createRemoteBtn');
    const createRemoteForm = document.getElementById('createRemoteForm');

    if (remotePlayBtn && remotePlayForm) {
        remotePlayBtn.addEventListener('click', function () {
            // Toggle display of remote play form
            if (remotePlayForm.style.display === 'none') {
                remotePlayForm.style.display = 'block';
                if (createRemoteForm) createRemoteForm.style.display = 'none';
            } else {
                remotePlayForm.style.display = 'none';
            }
        });
    }

    if (createRemoteBtn && createRemoteForm) {
        createRemoteBtn.addEventListener('click', function () {
            // Toggle display of create remote form
            if (createRemoteForm.style.display === 'none') {
                createRemoteForm.style.display = 'block';
                if (remotePlayForm) remotePlayForm.style.display = 'none';
            } else {
                createRemoteForm.style.display = 'none';
            }
        });
    }

    // Check for URL parameters for auto-joining
    const urlParams = new URLSearchParams(window.location.search);
    const joinGameId = urlParams.get('join');
    const playerName = urlParams.get('player');

    if (joinGameId) {
        // Auto-show the join form with pre-filled game ID
        if (remotePlayForm) {
            remotePlayForm.style.display = 'block';
            const gameIdInput = remotePlayForm.querySelector('input[name="gameId"]');
            if (gameIdInput) {
                gameIdInput.value = joinGameId;
            }
        }

        // Pre-fill player name if provided
        if (playerName) {
            const playerNameInput = document.querySelector('input[name="playerName"]');
            if (playerNameInput) {
                playerNameInput.value = playerName;
            }
        }
    }
});

// Copy functions for lobby sharing
function copyGameLink() {
    const gameLinkInput = document.getElementById('gameLink');
    gameLinkInput.select();
    gameLinkInput.setSelectionRange(0, 99999); // For mobile devices

    navigator.clipboard.writeText(gameLinkInput.value).then(function () {
        showCopyFeedback('Link copied to clipboard!');
    }).catch(function () {
        // Fallback for older browsers
        document.execCommand('copy');
        showCopyFeedback('Link copied to clipboard!');
    });
}

function copyGameId() {
    const gameIdInput = document.getElementById('gameId');
    gameIdInput.select();
    gameIdInput.setSelectionRange(0, 99999); // For mobile devices

    navigator.clipboard.writeText(gameIdInput.value).then(function () {
        showCopyFeedback('Game ID copied to clipboard!');
    }).catch(function () {
        // Fallback for older browsers
        document.execCommand('copy');
        showCopyFeedback('Game ID copied to clipboard!');
    });
}

function showCopyFeedback(message) {
    // Create or show feedback element
    let feedback = document.getElementById('copyFeedback');
    if (!feedback) {
        feedback = document.createElement('div');
        feedback.id = 'copyFeedback';
        feedback.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: #00ff00;
            color: #000;
            padding: 10px 20px;
            border-radius: 5px;
            z-index: 1000;
            font-weight: bold;
        `;
        document.body.appendChild(feedback);
    }

    feedback.textContent = message;
    feedback.style.display = 'block';

    setTimeout(() => {
        feedback.style.display = 'none';
    }, 3000);
}

