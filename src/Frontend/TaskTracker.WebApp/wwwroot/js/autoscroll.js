window.registerPageAutoScroll = () => {
    let scrollInterval;
    const speed = 15;
    const zoneSize = 100;

    const stopScroll = () => {
        clearInterval(scrollInterval);
        scrollInterval = null;
    };

    const startScroll = (stepX, stepY) => {
        if (scrollInterval) return;

        scrollInterval = setInterval(() => {
            window.scrollBy(stepX, stepY);
        }, 20);
    };

    document.addEventListener('dragover', (e) => {
        const viewportHeight = window.innerHeight;
        const viewportWidth = window.innerWidth;

        if (e.clientY < zoneSize) {
            stopScroll();
            startScroll(0, -speed);
        } else if (e.clientY > viewportHeight - zoneSize) {
            stopScroll();
            startScroll(0, speed);
        }
        else if (e.clientX < zoneSize) {
            stopScroll();
            startScroll(-speed, 0);
        } else if (e.clientX > viewportWidth - zoneSize) {
            stopScroll();
            startScroll(speed, 0);
        }
        else {
            stopScroll();
        }
    });

    document.addEventListener('dragend', stopScroll);
    document.addEventListener('drop', stopScroll);
};