document.addEventListener('DOMContentLoaded', () => {
    // Simple DOM references
    const fetchBtn = document.getElementById('fetchBtn');
    const responseContainer = document.getElementById('responseItems');
    const loading = document.getElementById('loading');

    // Add event listener for fetch button
    fetchBtn.addEventListener('click', fetchStreamData);

    async function fetchStreamData() {
        try {
            // Clear previous responses and show loading state
            responseContainer.innerHTML = '';
            loading.style.display = 'flex';
            fetchBtn.disabled = true;

            // Make the streaming API request
            const response = await fetch('/api/Stream/stream');

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Set up the stream reader
            const reader = response.body.getReader();
            const decoder = new TextDecoder();

            // Create a container for the live updates
            const liveUpdatesContainer = document.createElement('div');
            liveUpdatesContainer.className = 'live-updates';
            responseContainer.appendChild(liveUpdatesContainer);

            // Process the stream
            while (true) {
                const { value, done } = await reader.read();

                if (done) {
                    console.log("Stream complete");
                    break;
                }

                // Decode the chunk
                const chunk = decoder.decode(value, { stream: true });
                console.log("Received chunk:", chunk);

                // Display the chunk immediately
                displayChunk(chunk, liveUpdatesContainer);
            }

        } catch (error) {
            console.error('Error fetching stream:', error);
            responseContainer.innerHTML = `<div class="error">Error: ${error.message}</div>`;
        } finally {
            loading.style.display = 'none';
            fetchBtn.disabled = false;
        }
    }

    // Function to display each chunk as it arrives
    function displayChunk(chunk, container) {
        // Create a new element for this chunk
        const chunkElement = document.createElement('div');
        chunkElement.className = 'chunk-item new';

        try {
            // Try to parse the chunk as JSON
            let jsonContent;

            // Simple handling for basic JSON structure
            // This is a simplified approach and might need adjustments based on your exact stream format
            if (chunk.includes('{') && chunk.includes('}')) {
                // Extract content between curly braces
                const jsonStart = chunk.indexOf('{');
                const jsonEnd = chunk.lastIndexOf('}') + 1;
                const jsonString = chunk.substring(jsonStart, jsonEnd);

                try {
                    const jsonObj = JSON.parse(jsonString);
                    jsonContent = `<pre>${JSON.stringify(jsonObj, null, 2)}</pre>`;
                } catch (e) {
                    // If parsing fails, show raw content
                    jsonContent = `<pre>${escapeHtml(chunk)}</pre>`;
                }
            } else {
                // For non-JSON content, show as-is
                jsonContent = `<pre>${escapeHtml(chunk)}</pre>`;
            }

            // Add timestamp
            const timestamp = new Date().toLocaleTimeString();
            chunkElement.innerHTML = `
                <div class="chunk-header">
                    <span class="chunk-timestamp">${timestamp}</span>
                </div>
                <div class="chunk-content">${jsonContent}</div>
            `;

        } catch (error) {
            // If any error occurs, show raw chunk data
            chunkElement.innerHTML = `
                <div class="chunk-header">
                    <span class="chunk-timestamp">${new Date().toLocaleTimeString()}</span>
                </div>
                <div class="chunk-content"><pre>${escapeHtml(chunk)}</pre></div>
            `;
        }

        // Add to the container
        container.appendChild(chunkElement);

        // Scroll to the bottom to show latest chunk
        container.scrollTop = container.scrollHeight;

        // Remove the highlight after animation
        setTimeout(() => {
            chunkElement.classList.remove('new');
        }, 500);
    }

    // Helper function to escape HTML in raw content
    function escapeHtml(text) {
        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }
});