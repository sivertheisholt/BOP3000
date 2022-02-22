export interface UserProfile{
    email: string;
    username: string;
    age: number;
    location: string;
    information: {
        userDescription: string,
        userFavoriteGames: string,
    };
}