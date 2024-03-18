const isMobileDevice = () => {
    const screenWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    const mobileWidthThreshold = 768; // Adjust this threshold as needed

    return screenWidth < mobileWidthThreshold;
}

export default isMobileDevice;