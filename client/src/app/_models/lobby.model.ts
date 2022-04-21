export interface Lobby{
    id: number;
    maxUsers: number;
    title: string;
    startDate?: Date;
    finishDate?: Date;
    lobbyDescription? : string;
    gameId: number;
    gameName?: string;
    steamId: number;
    gameType: string;
    users: number[];
    adminUid: number;
    adminUsername?: string;
    adminProfilePic?: string;
    finished?: boolean;
    votes?: [{
        voterUid: number;
        votedUid: number;
        upvote: boolean;
    }];
    lobbyRequirement: {
        gender?: string;
    };
}