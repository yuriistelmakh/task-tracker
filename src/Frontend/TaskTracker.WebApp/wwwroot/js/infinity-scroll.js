window.observer = {
    element: null,
    dotnetRef: null,
    observerInstance: null,

    initialize: function (element, dotnetRef) {
        if (this.observerInstance) {
            this.observerInstance.disconnect();
            this.observerInstance = null;
        }

        this.element = element;
        this.dotnetRef = dotnetRef;

        var scrollParent = element.closest('.overflow-y-auto');

        var options = {
            root: scrollParent,
            rootMargin: '0px',
            threshold: 0.1
        };

        this.observerInstance = new IntersectionObserver(async (entries) => {
            if (entries[0].isIntersecting) {
                await dotnetRef.invokeMethodAsync('LoadMoreComments');
            }
        }, options);

        this.observerInstance.observe(element);
    },

    dispose: function () {
        if (this.observerInstance) {
            this.observerInstance.disconnect();
        }
        this.observerInstance = null;
        this.element = null;
        this.dotnetRef = null;
    }
};