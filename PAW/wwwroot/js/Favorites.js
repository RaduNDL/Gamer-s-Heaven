document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('.favorite-star').forEach(star => {
        star.addEventListener('click', async () => {
            const gameId = star.getAttribute('data-game');

            try {
                const res = await fetch('/Favorites/AddToWishlist', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ gameId: parseInt(gameId) })
                });

                if (res.ok) {
                    alert("Added to wishlist!");
                    star.classList.add('favorited');
                    star.innerHTML = "★";
                } else {
                    alert("Failed to add to wishlist.");
                }
            } catch (error) {
                console.error("Wishlist error:", error);
            }
        });
    });
});
