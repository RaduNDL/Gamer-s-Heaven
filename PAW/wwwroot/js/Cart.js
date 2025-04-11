document.addEventListener("DOMContentLoaded", () => {
    const buttons = document.querySelectorAll('.add-to-cart');

    buttons.forEach(button => {
        button.addEventListener('click', async function () {
            const card = this.closest('.game-card');
            const gameId = card?.querySelector('.favorite-star')?.dataset?.game;

            if (!gameId) {
                alert("Game ID not found.");
                return;
            }

            try {
                const response = await fetch('/Cart/AddToCart', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ gameId: parseInt(gameId) })
                });


                if (response.ok) {
                    const result = await response.json();
                    alert(result.message || "Added to cart!");
                } else {
                    alert("Failed to add to cart.");
                }
            } catch (error) {
                alert("Something went wrong.");
            }
        });
    });
});
