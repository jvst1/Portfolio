export default function () {
  return {
    // Public url
    publicUrl: process.env.BASE_URL,
    appUrl: process.env.VUE_APP_ROOT,

    // Animate scrollTop
    scrollTop(
      to,
      duration,
      element = document.scrollingElement || document.documentElement
    ) {
      if (element.scrollTop === to) return;
      const start = element.scrollTop;
      const change = to - start;
      const startDate = +new Date();

      // t = current time; b = start value; c = change in value; d = duration
      const easeInOutQuad = (t, b, c, d) => {
        t /= d / 2;
        if (t < 1) return (c / 2) * t * t + b;
        t--;
        return (-c / 2) * (t * (t - 2) - 1) + b;
      };

      const animateScroll = () => {
        const currentDate = +new Date();
        const currentTime = currentDate - startDate;
        element.scrollTop = parseInt(
          easeInOutQuad(currentTime, start, change, duration)
        );
        if (currentTime < duration) {
          requestAnimationFrame(animateScroll);
        } else {
          element.scrollTop = to;
        }
      };

      animateScroll();
    },
  };
}
