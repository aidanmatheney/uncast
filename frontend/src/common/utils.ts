export const formatSeconds = (seconds: number) => {
  const date = new Date(0);
  date.setSeconds(seconds);
  return date.toISOString().substr(11, 8);
};

export const formatUnixDate = (unixDate: number) => {
  return new Date(unixDate).toLocaleDateString();
};

export const getUncastDb = () => {
  const openRequest = indexedDB.open('uncast', 2);

  const promise: Promise<IDBDatabase> = new Promise((resolve, reject) => {
    openRequest.onsuccess = () => {
      resolve(openRequest.result);
    };

    openRequest.onblocked = () => {
      reject(openRequest.error);
    };

    openRequest.onupgradeneeded = () => {
      const db = openRequest.result;
      db.createObjectStore('files');
    }
  });
  return promise;
};

export const getUncastDbStore = async (storeName: string) => {
  const db = await getUncastDb();

  const transaction = db.transaction(storeName, 'readwrite');
  const store = transaction.objectStore(storeName);
  return store;
};

export const getUncastDbValue = async <V>(storeName: string, key: string) => {
  const store = await getUncastDbStore(storeName);
  const request = store.get(key);
  const promise: Promise<V> = new Promise((resolve, reject) => {
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => reject(request.error);
  });
  return await promise;
};

export const putUncastDbValue = async <V>(storeName: string, key: string, value: V) => {
  const store = await getUncastDbStore(storeName);
  const request = store.put(value, key);
  const promise: Promise<void> = new Promise((resolve, reject) => {
    request.onsuccess = () => resolve();
    request.onerror = () => reject(request.error);
  });
  return await promise;
};

export const getUncastDbFile = (id: string) => getUncastDbValue<string>('files', id);
export const putUncastDbFile = (id: string, data: string) => putUncastDbValue<string>('files', id, data);
