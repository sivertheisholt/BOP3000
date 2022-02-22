export interface Lobby{
    gameId: number;
    title: string;
    lobbyDescription? : string;
    gameType: string;
    maxUsers: number;
    lobbyRequirement: {
        gender?: string;
    };
}