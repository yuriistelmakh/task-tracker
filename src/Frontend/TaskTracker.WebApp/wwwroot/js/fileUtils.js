window.openFile = function (url, fileName) {
    console.log('openFile called with:', url, fileName);
    try {
        window.open(url, '_blank');
    } catch (error) {
        console.error('Error opening file:', error);
    }
};

window.downloadFile = async function (url, fileName) {
    console.log('downloadFile called with:', url, fileName);
    try {
        const response = await fetch(url);
        const blob = await response.blob();
        const blobUrl = URL.createObjectURL(blob);

        const link = document.createElement('a');
        link.href = blobUrl;
        link.download = fileName || 'download';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        setTimeout(() => URL.revokeObjectURL(blobUrl), 100);
    } catch (error) {
        console.error('Download failed:', error);
    }
};

