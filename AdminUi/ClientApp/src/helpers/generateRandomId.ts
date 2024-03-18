function generateRandomId(length: number): string {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';

    // Generate the random string
    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * characters.length);
        result += characters.charAt(randomIndex);
    }

    // Get the current timestamp
    const timestamp = Date.now().toString();

    // Concatenate the timestamp with the random string
    const idWithTimestamp = `${result}-${timestamp}`;

    return idWithTimestamp;
}

export default generateRandomId;