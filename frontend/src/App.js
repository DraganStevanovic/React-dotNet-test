import React from "react";
import "./App.css";

const initState = {
  tasks: [],
  limit: 10,
  total: 0,
  offset: 0,
  totalPage: 1,
  loadRequest: false,
  loadFailure: false,
  loadSuccess: false,
};

const handlers = {
  LOAD_REQUEST(state, action) {
    if (state.loadRequest) {
      return state;
    }
    return {
      ...state,
      loadRequest: true,
    };
  },

  LOAD_FAILURE(state, action) {
    if (!state.loadRequest) {
      return state;
    }
    return {
      ...state,
      loadRequest: false,
      loadFailure: true,
    };
  },

  LOAD_SUCCESS(state, action) {
    if (!state.loadRequest) {
      return state;
    }
    return {
      ...state,
      tasks: action.payload.tasks,
      total: action.payload.total,
      offset: action.payload.offset,
      limit: action.payload.limit,
      totalPage: Math.ceil(action.payload.total / action.payload.limit),
      loadRequest: false,
      loadFailure: false,
      loadSuccess: true,
      loaded: true,
    };
  },
};

function reducer(state, action) {
  const { type } = action;
  const { [type]: handler } = handlers;
  if (typeof handler === "function") {
    return handler(state, action);
  }
  return state;
}

function App() {
  const [state, dispatch] = React.useReducer(reducer, initState);

  const [page] = React.useState(1);

  const { tasks } = state;

  const loadTasks = React.useCallback(
    async (offset, limit) => {
      dispatch({
        type: "LOAD_REQUEST",
      });
      const response = await fetch(
        `http://localhost:5000/api/tasks?offset=${offset}&limit=${limit}`
      );
      if (response.status !== 200) {
        dispatch({
          type: "LOAD_FAILURE",
        });
      } else {
        const responseJSON = await response.json();
        dispatch({
          type: "LOAD_SUCCESS",
          payload: {
            total: responseJSON.payload.totalCount,
            offset,
            limit,
            tasks: responseJSON.payload.tasks,
          },
        });
      }
    },
    [dispatch]
  );

  React.useEffect(() => {
    const offset = (page - 1) * 10;
    loadTasks(offset, 10);
  }, [loadTasks, page]);

  return (
    <div className="App">
      <table cellPadding={0} cellSpacing={0} border={"1"}>
        <thead>
          <th>Name</th>
          <th>Description</th>
          <th>Status</th>
          <th>Creation Time</th>
          <th>Updation Time</th>
        </thead>
        <tbody>
          {tasks.map((task) => {
            return (
              <tr key={`${task.id}`}>
                <td>{task.name}</td>
                <td>{task.description}</td>
                <td>{task.status}</td>
                <td>{task.createdAt}</td>
                <td>{task.updatedAt}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}

export default App;
